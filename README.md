# 2020-unity-common
Common code and assets for Unity games.

# Usage

- Create a Unity project through Unity / Unity Hub.
- Inside the project folder, do the following:
```bash
  git init
  cd Assets
  git submodule add https://github.com/JoseHdez2/2020-unity-common.git
  cp 2020-unity-common/.gitignore ..
  cd 2020-unity-common
  git checkout --track origin/setup
```
- Open Unity, and click on `Tools > Setup Unity Common` and wait for it to finish.
- Go to `Edit > Project Settings > Player > Other Settings > Active Input Handling`.
  - Set it to `Both`. Editor will restart.
- Go back to the master branch:
```
  git checkout master
```
