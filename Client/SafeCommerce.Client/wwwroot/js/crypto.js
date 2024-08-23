class KeyPairs {
    constructor(publicKey, privateKey) {
        this.publicKey = publicKey;
        this.privateKey = privateKey;
    }
}

async function hashPasswordAndUserID(password, userId) {
    const combined = `${password}:${userId}`;
    const encoder = new TextEncoder();
    const data = encoder.encode(combined);

    const hashBuffer = await crypto.subtle.digest('SHA-256', data);
    return new Uint8Array(hashBuffer);
}

async function generateEncryptionKeyPair(password, userId) {
    const seed = await hashPasswordAndUserID(password, userId);
    const keyPair = nacl.box.keyPair.fromSecretKey(seed.slice(0, 32));

    const publicKeyBase64 = nacl.util.encodeBase64(keyPair.publicKey);
    const secretKeyBase64 = nacl.util.encodeBase64(keyPair.secretKey);

    return new KeyPairs(publicKeyBase64, secretKeyBase64);
}

async function generateSigningKeyPair(password, userId) {
    const seed = await hashPasswordAndUserID(password, userId);

    const signingKeyPair = nacl.sign.keyPair.fromSeed(seed.slice(0, 32));

    const publicKeyBase64 = nacl.util.encodeBase64(signingKeyPair.publicKey);
    const privateKeyBase64 = nacl.util.encodeBase64(signingKeyPair.secretKey);

    return {
        publicKey: publicKeyBase64,
        privateKey: privateKeyBase64
    };
}

async function generateSigningKeyPairWithNoSeed() {
    const signingKeyPair = nacl.sign.keyPair();

    const publicKeyBase64 = nacl.util.encodeBase64(signingKeyPair.publicKey);
    const privateKeyBase64 = nacl.util.encodeBase64(signingKeyPair.secretKey);

    return {
        publicKey: publicKeyBase64,
        privateKey: privateKeyBase64
    };
}

async function generateSymmetricKey() {
    const key = await crypto.subtle.generateKey(
        {
            name: "AES-GCM",
            length: 256,
        },
        true,
        ["encrypt", "decrypt"]
    );
    const exportedKey = await crypto.subtle.exportKey("raw", key);
    return nacl.util.encodeBase64(new Uint8Array(exportedKey));
}

async function encryptDataWithSymmetricKey(data, symmetricKeyBase64, nonce) {
    const key = await crypto.subtle.importKey(
        "raw",
        nacl.util.decodeBase64(symmetricKeyBase64),
        { name: "AES-GCM" },
        false,
        ["encrypt"]
    );

    const encryptedData = await crypto.subtle.encrypt(
        { name: "AES-GCM", iv: nonce },
        key,
        new TextEncoder().encode(data)
    );

    return {
        encryptedData: nacl.util.encodeBase64(new Uint8Array(encryptedData))
    };
}

async function encryptSymmetricKeyForUser(symmetricKeyBase64, recipientPublicKeyBase64, senderPrivateKeyBase64) {
    const recipientPublicKey = nacl.util.decodeBase64(recipientPublicKeyBase64);
    const symmetricKey = nacl.util.decodeBase64(symmetricKeyBase64);
    const senderPrivateKey = nacl.util.decodeBase64(senderPrivateKeyBase64);

    const nonce = nacl.randomBytes(nacl.box.nonceLength);

    const encryptedSymmetricKey = nacl.box(symmetricKey, nonce, recipientPublicKey, senderPrivateKey);

    return {
        encryptedSymmetricKeyBase64: nacl.util.encodeBase64(encryptedSymmetricKey),
        nonceBase64: nacl.util.encodeBase64(nonce)
    };
}

async function decryptSymmetricKeyWithPrivateKey(encryptedSymmetricKeyBase64, recipientPrivateKeyBase64, senderPublicKeyBase64, nonceBase64) {
    const encryptedSymmetricKey = nacl.util.decodeBase64(encryptedSymmetricKeyBase64);
    const recipientPrivateKey = nacl.util.decodeBase64(recipientPrivateKeyBase64);
    const senderPublicKey = nacl.util.decodeBase64(senderPublicKeyBase64);
    const nonce = nacl.util.decodeBase64(nonceBase64);

    const symmetricKey = nacl.box.open(encryptedSymmetricKey, nonce, senderPublicKey, recipientPrivateKey);

    if (!symmetricKey) {
        throw new Error('Failed to decrypt the symmetric key');
    }

    return nacl.util.encodeBase64(symmetricKey);
}

async function decryptDataWithSymmetricKey(data, symmetricKey, nonce) {
    try {
        const decryptedData = await crypto.subtle.decrypt(
            { name: "AES-GCM", iv: nonce },
            symmetricKey,
            nacl.util.decodeBase64(data)
        );

        return new TextDecoder().decode(decryptedData);

    } catch (e) {
        console.log(e);
    }
}

async function decryptPPK(encryptedObject, password) {
    try {
        const { encryptedData, iv } = encryptedObject;

        const encoder = new TextEncoder();

        const keyMaterial = await crypto.subtle.importKey(
            "raw",
            encoder.encode(password),
            "PBKDF2",
            false,
            ["deriveKey"]
        );

        const key = await crypto.subtle.deriveKey(
            {
                name: "PBKDF2",
                salt: new Uint8Array(0),
                iterations: 100000,
                hash: "SHA-256"
            },
            keyMaterial,
            { name: "AES-GCM", length: 256 },
            false,
            ["decrypt"]
        );

        const decryptedData = await crypto.subtle.decrypt(
            {
                name: "AES-GCM",
                iv: new Uint8Array(iv)
            },
            key,
            new Uint8Array(encryptedData)
        );

        return new TextDecoder().decode(decryptedData);
    } catch (e) {
        console.log(e);
    }
}

async function decryptEncryptedFile(encryptedObject, password) {
    const { encryptedData, iv } = encryptedObject;

    const encoder = new TextEncoder();

    const keyMaterial = await crypto.subtle.importKey(
        "raw",
        encoder.encode(password),
        "PBKDF2",
        false,
        ["deriveKey"]
    );

    const key = await crypto.subtle.deriveKey(
        {
            name: "PBKDF2",
            salt: new Uint8Array(0),
            iterations: 100000,
            hash: "SHA-256"
        },
        keyMaterial,
        { name: "AES-GCM", length: 256 },
        false,
        ["decrypt"]
    );

    const decryptedData = await crypto.subtle.decrypt(
        {
            name: "AES-GCM",
            iv: new Uint8Array(iv)
        },
        key,
        new Uint8Array(encryptedData)
    );

    return decryptedData;
}

function hashData(data) {
    return nacl.hash(data);
}

function signData(data, signingPrivateKey) {
    return nacl.sign.detached(data, signingPrivateKey);
}

function verifySignature(hashedData, signature, signingPublicKey) {
    return nacl.sign.detached.verify(hashedData, signature, signingPublicKey);
}

async function importSymmetricKey(symmetricKeyBase64) {
    return crypto.subtle.importKey(
        "raw",
        nacl.util.decodeBase64(symmetricKeyBase64),
        { name: "AES-GCM" },
        false,
        ["encrypt", "decrypt"]
    );
}