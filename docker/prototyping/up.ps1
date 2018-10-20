Copy-Item ../../src/Spectaris/bin/Debug/net462/* ./rootfs/
Copy-Item ../../install-module.ps1 ./rootfs/

docker-compose up --build