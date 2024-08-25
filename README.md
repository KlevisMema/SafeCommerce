# Security Computer Project

## Overview
This document outlines the requirements, features, and instructions for implementing a secure client/server system for Shop and Items.

## Contributors
Mema Klevis 582513 / Namange Esembe Ndjanang Njiki - 583634.

## Table of Contents
1. [Introduction](#1-introduction)
2. [System Characteristics](#2-system-characteristics)
3. [Features](#3-features)
4. [Technologies Used](#4-technologies-used)
5. [Requirements](#5-requirements)
6. [Setup Instructions](#6-setup-instructions)

## 1. Introduction
The project aims to implement a secure system for handling common expenses within a group, emphasizing security aspects in data storage and transmission. This project only runs on x64
Windows 10.

### Key Points
- Focus on security protocols and techniques.
- Freedom in choosing languages and protocols but with responsibility.
- Penalties for choices that compromise security.

## 2. System Characteristics
The system follows a client/server architecture with specific user and server roles. Key tasks include user registration, authentication, and various operations for authenticated users.

### Server
- Not a trusted entity.
- Secure transfer and ownership verification mechanisms.

### Items
- Users can create items for a shop or individually.
- Only the item owner can manage item membership.

### Shop
- Users can create shops to share items.
- Only the group owner can manage shop membership.
- User can invite other users to his public/private shop

## 3. Features
This section outlines the high-level features and protocols for secure data exchange, storage, and user activities.

### User Registration, Authentication, and Revocation
- Secure credentials generation for user registration.
- Users can log in from different devices.

### Shops
- Any user can create a public or private shop.
- Public Shops are subject for moderation.
- Owner of the shops can edit/delete or invite other users in it.
- Owner of the shop can add items in the shop.

### Item
- Any user can create a public or private item.
- Public items are subject for moderation.
- Owner of the item can edit/delete or invite other users in it.

## 4. Technologies Used
- **.NET 8 Web API**: Used for building the server-side application.
- **Blazor**: Utilized for building interactive web applications.
- **SQL Server**: Chosen as the relational database management system.
- **SendGrid**: Integrated for handling email functionalities.
- **Serilog and Seq**: Employed for structured logging.
- **Libraries**: Mediator, Automapper, JWT, Entity Framework, HTTP Client, Mud Blazor.

## 5. Requirements
Ensure the following components are installed and configured on your system:

- **.NET 8 SDK**: [Download .NET SDK] https://dotnet.microsoft.com/en-us/download
- **SQL Server**: Installed and running. https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- **SSMS**: Installed and running. https://aka.ms/ssmsfullsetup
- **Seq**: Running on http://localhost:5341/ | Download in https://datalust.co/download/begin?version=2023.4.10219

## 6. Setup Instructions

## Environment Variables:
   - **.SAFE_SHARE_HMAC** : ......
    To generate a HMAC you need to run the project in server directory called SafeCommerce.InternalCrypto.
    Grab the value and place it under the key SAFE_SHARE_HMAC.
   - **.SendGridKey_SafeCommerce** : ....
   A send grid key is necessary to send email when registering in the app and other stuff.
   https://sendgrid.com/en-us?utm_source=google&utm_medium=cpc&utm_term=sendgrid&utm_campaign=G_S_EMEA_TSG_Brand_T1_NV&cq_plac=&cq_net=g&cq_pos=&cq_med=&cq_plt=gp&gad_source=1&gclid=Cj0KCQjwrKu2BhDkARIsAD7GBoswiYF4bzWbSjosO9vdwlq8mIJQTtXd3EhZ6kWoLY5cRSQx9F0RViAaApkgEALw_wcB
   - **.MAIN_API_PUBLIC_KEY_HASH** : .....
   For this env you need to extract the public key of the certeficate of the SafeCommerce.API in server and store its value. This is done for PKP (Public Key Pinning)

## Run Project
To run the project 3 projects need to start 

in Client : SafeCommerce.Client & SafeCommerce.ProxyApi
in Server : SafeCommerce.API 
In 3 different terminals inside these projects run the command : donet watch run, then navigate to the client website by taking the link
in the terminal when you ran the .Client 