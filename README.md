<div align="center">
<br/>
<img src="https://user-images.githubusercontent.com/6564442/186253927-9662c3e6-b395-49f0-9a56-42eaab54e109.png" width="144px" height="144px"/>

### Laser Project

![Unity](https://img.shields.io/badge/unity-2021.3.1f1-brightgreen?style=for-the-badge&logo=unity&logoColor=white)
</div>

## :book: Overview

This repository contains the code for **Laser Project**, a Unity game where a select few people
at [ORY Games](https://github.com/KanabaGames) can collaborate and test out new game development ideas.

## :closed_book: Things to know

### Settings up DOTween

When opening the project for the first time, you might encounter errors regarding DOTWeen. If this happens, do the
following:

1. Open the project (Ignore any errors)
2. Open the **DOTween Utility Panel** from the **Tools menu**
3. Select **Setup DOTween...** from the panel that appears.
4. Press **Apply**

DOTWeen should now be configured successfully and the project should work correctly now.

### Settings up Git

#### Git LFS

This repo makes use of [Git LFS](https://git-lfs.github.com/), which allows support for adding large files (audio,
video, artwork, etc...) to a git repo.

To set it up, do the following:

1. Install [Git LFS](https://git-lfs.github.com/) for your operating system
2. Run `git lfs install` once to activate it (you only need to do this once per user account)

#### Force Git to use LF instead of CRLF (Windows)

This repo uses `LF` as line breaks. If you're running on Windows, you might face an issue when opening PRs, or doing any
commits in general, that any file you edit will use `CRLF` instead of `LF`.

This is most likely an issue with Git on Windows.

To fix it, run the following:

```sh
 git config --global core.eol lf
 git config --global core.autocrlf input
```

You can read about the difference between using `LF` and `CRLF` as
linebreaks [in this stackoverflow post](https://stackoverflow.com/questions/1552749/difference-between-cr-lf-lf-and-cr-line-break-types)
.

### CI / CD

This repo makes heavy usage of the CI / CD workflows provide by [GameCI](https://game.ci/).

All the available workflows can be found in the `.github/workflows` directory.

### Unity Atoms

This project makes heavy use of [Unity Atoms](https://unity-atoms.github.io/unity-atoms/), an event based system that
encourages data-driven design.

To understand the reasoning behind using this package, feel free to watch the following two videos (If you're short on
time, only watch the first video):

1. [Important] [Ryan Hipple's talk at Unity Austin 2017](https://www.youtube.com/watch?v=raQ3iHhE_Kk)
2. [Richard Fine's talk at Unite 2016](https://www.youtube.com/watch?v=6vmRwLYWNRo)

### Shadow Casting for Tilemaps

Unity doesn't support adding shadow casting to tilemaps by default. A workaround implemented in this repo is the script
described
in [this forum post](https://forum.unity.com/threads/script-for-generating-shadowcaster2ds-for-tilemaps.906767/)

_TLDR: If you want to generate shadow caster for a tilemap, click on `Tools` > `Generate Shadow Casters` and shadow
caster objects will be created for the tilemaps in your current scene._

### Asset Usage Detector

This project has [Asser Usage Detector](https://github.com/yasirkula/UnityAssetUsageDetector) installed. To make use of
it, you can do one of the following:

- Open Window - Asset Usage Detector window, configure the settings and hit GO!
- Right click an object and select Search For References

### Commit Convention

Please use the [angular commit convention](https://www.conventionalcommits.org/en/v1.0.0-beta.4/#summary) when adding
new commits.

Not only does it make your commit more clear, but it also plays a key role in
how [semantic-release](https://github.com/semantic-release/semantic-release) (see below) determines the next version
number for a release.

#### Commit Message Header

```
<type>(<scope>): <short summary>
  │       │             │
  │       │             └─⫸ Summary in present tense. Not capitalized. No period at the end.
  │       │
  │       └─⫸ Commit Scope: animations|bazel|benchpress|common|compiler|compiler-cli|core|
  │                          elements|forms|http|language-service|localize|platform-browser|
  │                          platform-browser-dynamic|platform-server|router|service-worker|
  │                          upgrade|zone.js|packaging|changelog|dev-infra|docs-infra|migrations|
  │                          ngcc|ve
  │
  └─⫸ Commit Type: build|ci|docs|feat|fix|perf|refactor|test
```

The `<type>` and `<summary>` fields are mandatory, the `(<scope>)` field is optional.

#### Type

Must be one of the following:

- **build**: Changes that affect the build system or external dependencies (example scopes: gulp, broccoli, npm)
- **ci**: Changes to our CI configuration files and scripts (example scopes: Circle, BrowserStack, SauceLabs)
- **docs**: Documentation only changes
- **feat**: A new feature
- **fix**: A bug fix
- **perf**: A code change that improves performance
- **refactor**: A code change that neither fixes a bug nor adds a feature
- **test**: Adding missing tests or correcting existing tests

### Releases

Releases are handled automatically using [semantic-release](https://github.com/semantic-release/semantic-release). Its
configuration can be found in `.releaserc.json`.

Here's how releases are currently made:

- Pushing any commit to the `alpha` branch will create a new **alpha** release. These will be tagged as `v1.0.0-alpha.1`
  , `v1.0.0-alpha.2`, `v1.0.0-alpha.3`, etc...
- Pushing any commit to the `main` branch will create a new (ordinary) release. These will be tagged as `v1.0.0`
  , `v1.0.1`, `v1.1.0`, `v2.1.0` etc...

## :toilet: (Useful) Links and Guides

- [Gaming Deisgn Patterns](https://gameprogrammingpatterns.com/contents.html)

### Git Setup

- [https://thoughtbot.com/blog/how-to-git-with-unity](https://thoughtbot.com/blog/how-to-git-with-unity)
- [https://unityatscale.com/](https://unityatscale.com/)

### 2D Controllers

- [https://github.com/cjddmut/Unity-2D-Platformer-Controller](https://github.com/cjddmut/Unity-2D-Platformer-Controller)
- [https://github.com/prime31/CharacterController2D/blob/master/Assets/CharacterController2D/CharacterController2D.cs](https://github.com/prime31/CharacterController2D/blob/master/Assets/CharacterController2D/CharacterController2D.cs)
