sudo snap remove fos-markirovka
rm fos-markirovka_2024.09.20_arm64.snap
rm fos-markirovka_2024.09.20_armhf.snap
rm fos-markirovka_2024.09.20_amd64.snap

#dotnet publish fos.markirovka.csproj --configuration Release --self-contained

#snapcraft clean
snapcraft --target-arch arm64
snapcraft --target-arch armhf
snapcraft --target-arch amd64

sudo snap install --devmode --edge fos-markirovka_2024.09.20_arm64.snap
sudo snap install --devmode --edge fos-markirovka_2024.09.20_armhf.snap
sudo snap install --devmode --edge fos-markirovka_2024.09.20_amd64.snap