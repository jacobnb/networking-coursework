#!/usr/bin/env bash
mkdir SDKs
cd SDKs
git clone https://github.com/dbucksteinccorg/DevSDKs-GPRO-2020.git
cd DevSDKs-GPRO-2020
git checkout dev-sdks
cd DevSDKs
./dev_install.bat
PAUSE