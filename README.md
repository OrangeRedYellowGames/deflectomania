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

``` bash
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

## :toilet: (Useful) Links and Guides

- [Gaming Deisgn Patterns](https://gameprogrammingpatterns.com/contents.html)

### Git Setup

- [https://thoughtbot.com/blog/how-to-git-with-unity](https://thoughtbot.com/blog/how-to-git-with-unity)
- [https://unityatscale.com/](https://unityatscale.com/)

### 2D Controllers

- [https://github.com/cjddmut/Unity-2D-Platformer-Controller](https://github.com/cjddmut/Unity-2D-Platformer-Controller)
- [https://github.com/prime31/CharacterController2D/blob/master/Assets/CharacterController2D/CharacterController2D.cs](https://github.com/prime31/CharacterController2D/blob/master/Assets/CharacterController2D/CharacterController2D.cs)
