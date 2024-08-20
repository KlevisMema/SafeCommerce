async function openDatabase() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open("cryptoKeysDatabase", 1);

        request.onerror = (event) => {
            reject('IndexedDB database error: ', event.target.errorCode);
        };

        request.onupgradeneeded = (event) => {
            const db = event.target.result;

            if (!db.objectStoreNames.contains('UserKeys')) {
                db.createObjectStore("UserKeys", { keyPath: "id" });
            }

            if (!db.objectStoreNames.contains('ShopKeys')) {
                db.createObjectStore("ShopKeys", { keyPath: "shopId" });
            }

            if (!db.objectStoreNames.contains('ItemKeys')) {
                db.createObjectStore("ItemKeys", { keyPath: "itemId" });
            }
        };

        request.onsuccess = (event) => {
            resolve(event.target.result);
        };
    });
}

async function saveUserKeysInDatabase(userId, keyPair, myDevice, password, keyPairSignature, signature) {
    const db = await openDatabase();
    const transaction = db.transaction("UserKeys", "readwrite");
    const store = transaction.objectStore("UserKeys");

    const request = store.put({
        id: userId,
        keyPair: keyPair,
        keyPairSignature: keyPairSignature,
        signature: signature
    });

    return new Promise((resolve, reject) => {
        request.onsuccess = async () => {
            try {
                if (myDevice) {
                    await downloadKeyPairAsFile(userId, keyPair, password, keyPairSignature, signature);
                }
                resolve();
            } catch (err) {
                console.error('Error during downloadKeyPairAsFile: ', err);
                reject(err);
            }
        };
        request.onerror = (event) => {
            console.error('Error saving the key pair: ', event.target.errorCode);
            reject('Error saving the key pair: ' + event.target.errorCode);
        };
    });
}

async function saveItemKeysInDatabase(itemId, ownerId, encryptedSymmetricKey, signature, relatedKeys) {
    const db = await openDatabase();
    const transaction = db.transaction("ItemKeys", "readwrite");
    const store = transaction.objectStore("ItemKeys");

    const request = store.put({
        itemId: itemId,
        ownerId: ownerId,
        encryptedSymmetricKey: encryptedSymmetricKey,
        signature: signature,
        relatedKeys: relatedKeys
    });

    return new Promise((resolve, reject) => {
        request.onsuccess = () => resolve();
        request.onerror = (event) => reject('Error saving the item keys: ' + event.target.errorCode);
    });
}

async function saveShopKeysInDatabase(shopId, ownerId, encryptedSymmetricKey, signature, relatedKeys, nonce, keyNonce) {
    const db = await openDatabase();
    const transaction = db.transaction("ShopKeys", "readwrite");
    const store = transaction.objectStore("ShopKeys");

    const request = store.put({
        shopId: shopId,
        ownerId: ownerId,
        encryptedSymmetricKey: encryptedSymmetricKey,
        signature: signature,
        relatedKeys: relatedKeys,
        dataNonce: nonce,
        keyNonce: keyNonce
    });

    return new Promise((resolve, reject) => {
        request.onsuccess = () => resolve();
        request.onerror = (event) => reject('Error saving the shop keys: ' + event.target.errorCode);
    });
}

async function getUserKeyPairFromDatabase(userId) {
    try {
        const db = await openDatabase();
        const transaction = db.transaction("UserKeys", "readonly");
        const store = transaction.objectStore("UserKeys");

        const request = store.get(userId);
        return new Promise((resolve, reject) => {
            request.onsuccess = (event) => {
                const result = event.target.result;
                if (result && result.keyPair && result.signature && result.keyPairSignature) {
                    resolve(result);
                } else {
                    resolve(null);
                }
            };
            request.onerror = (event) => reject('Error retrieving the user key pair: ' + event.target.errorCode);
        });
    } catch (error) {
        console.error('Database error: ', error);
        throw new Error('Error opening the database');
    }
}

async function deleteUserKeyPairFromDatabase(userId) {
    try {
        const db = await openDatabase();
        const transaction = db.transaction("UserKeys", "readwrite");
        const store = transaction.objectStore("UserKeys");

        const request = store.delete(userId);

        return new Promise((resolve, reject) => {
            request.onsuccess = () => {
                console.log(`User key pair for user ${userId} deleted successfully.`);
                resolve(true);
            };
            request.onerror = (event) => {
                console.error('Error deleting the user key pair: ', event.target.errorCode);
                reject(false);
            };
        });
    } catch (error) {
        console.error('Database error: ', error);
        throw new Error('Error opening the database');
    }
}

async function getShopKeyPairFromDatabase(shopId) {
    try {
        const db = await openDatabase();
        const transaction = db.transaction("ShopKeys", "readonly");
        const store = transaction.objectStore("ShopKeys");

        const request = store.get(shopId);
        return new Promise((resolve, reject) => {
            request.onsuccess = (event) => {
                const result = event.target.result;
                if (result) {
                    resolve(result);
                } else {
                    resolve(null);
                }
            };
            request.onerror = (event) => reject('Error retrieving the shop key pair: ' + event.target.errorCode);
        });
    } catch (error) {
        console.error('Database error: ', error);
        throw new Error('Error opening the database');
    }
}

async function deleteShopKeyPairFromDatabase(shopId) {
    try {
        const db = await openDatabase();
        const transaction = db.transaction("ShopKeys", "readwrite");
        const store = transaction.objectStore("ShopKeys");

        const request = store.delete(shopId);

        return new Promise((resolve, reject) => {
            request.onsuccess = () => {
                console.log(`Shop key pair for shop ${shopId} deleted successfully.`);
                resolve(true);
            };
            request.onerror = (event) => {
                console.error('Error deleting the shop key pair: ', event.target.errorCode);
                reject(false);
            };
        });
    } catch (error) {
        console.error('Database error: ', error);
        throw new Error('Error opening the database');
    }
}

async function getItemKeyPairFromDatabase(itemId) {
    try {
        const db = await openDatabase();
        const transaction = db.transaction("ItemKeys", "readonly");
        const store = transaction.objectStore("ItemKeys");

        const request = store.get(itemId);
        return new Promise((resolve, reject) => {
            request.onsuccess = (event) => {
                const result = event.target.result;
                if (result && result.keyPair && result.signature && result.keyPairSignature) {
                    resolve(result);
                } else {
                    resolve(null);
                }
            };
            request.onerror = (event) => reject('Error retrieving the item key pair: ' + event.target.errorCode);
        });
    } catch (error) {
        console.error('Database error: ', error);
        throw new Error('Error opening the database');
    }
}

async function deleteItemKeyPairFromDatabase(itemId) {
    try {
        const db = await openDatabase();
        const transaction = db.transaction("ItemKeys", "readwrite");
        const store = transaction.objectStore("ItemKeys");

        const request = store.delete(itemId);

        return new Promise((resolve, reject) => {
            request.onsuccess = () => {
                console.log(`Item key pair for item ${itemId} deleted successfully.`);
                resolve(true);
            };
            request.onerror = (event) => {
                console.error('Error deleting the item key pair: ', event.target.errorCode);
                reject(false);
            };
        });
    } catch (error) {
        console.error('Database error: ', error);
        throw new Error('Error opening the database');
    }
}

async function encryptPPK(data, password) {
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
        ["encrypt"]
    );

    const iv = crypto.getRandomValues(new Uint8Array(12));

    const encryptedData = await crypto.subtle.encrypt(
        {
            name: "AES-GCM",
            iv: iv
        },
        key,
        encoder.encode(data)
    );

    return {
        encryptedData: new Uint8Array(encryptedData),
        iv: iv
    };
}

async function downloadKeyPairAsFile(userId, keyPair, password, keyPairSignature, signature) {
    const keyPairJson = JSON.stringify(keyPair);
    const keyPairSignatureJson = JSON.stringify(keyPairSignature);
    const signatureJson = JSON.stringify(signature);

    const { encryptedData: encryptedKeyPair, iv: keyPairIv } = await encryptPPK(keyPairJson, password);
    const { encryptedData: encryptedSignature, iv: signatureIv } = await encryptPPK(signatureJson, password);
    const { encryptedData: encryptedKeyPairSignature, iv: keyPairSignatureIv } = await encryptPPK(keyPairSignatureJson, password);

    const encryptedFileData = {
        encryptedKeyPair: Array.from(encryptedKeyPair),
        keyPairIv: Array.from(keyPairIv),
        encryptedSignature: Array.from(encryptedSignature),
        signatureIv: Array.from(signatureIv),
        encryptedKeyPairSignature: Array.from(encryptedKeyPairSignature),
        keyPairSignatureIv: Array.from(keyPairSignatureIv)
    };

    const blob = new Blob([JSON.stringify(encryptedFileData)], { type: "application/json" });

    const link = document.createElement("a");
    link.download = `${userId}_keypair.json`;
    link.href = URL.createObjectURL(blob);

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}