sudo snap remove fos-markirovka
rm fos-markirovka_2024.09.20_arm64.snap
rm fos-markirovka_2024.09.20_armhf.snap
rm fos-markirovka_2024.09.20_amd64.snap

dotnet publish --configuration Release --runtime linux-arm64 --self-contained
dotnet publish --configuration Release --runtime linux-arm --self-contained
dotnet publish --configuration Release --runtime linux-amd64 --self-contained

sudo snapcraft clean
sudo snapcraft

sudo snap install --devmode --edge fos-markirovka_2024.09.20_arm64.snap
sudo snap install --devmode --edge fos-markirovka_2024.09.20_armhf.snap
sudo snap install --devmode --edge fos-markirovka_2024.09.20_amd64.snap