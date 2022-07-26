# Network-Based-FPS
第一次尝试使用Unity开发网络游戏，客户端框架暂定为[GameFramework](https://gameframework.cn/)

## 策划方案
### 一、 游戏概述

1、 游戏暂定为第一人称多人射击游戏<br>
2、 可以实现多人在线游玩<br>
3、 游戏模式设定为对战和生化（暂先实现对战模式）<br>
4、 提前设定好枪支，后续进行类似CSgo的枪支购买功能<br>

### 二、 游戏机制
#### 1、玩法和规则<br>
**对战模式**：<br>
- 单人或者团队模式进行对战，谁先拿到设置的比分，就取得胜利。<br>

**生化模式**：<br>
- 僵尸玩家只能近战攻击，若攻击到人类玩家则将其转化为僵尸阵营，僵尸死亡不能复活，最后场上仅存的阵营取得胜利。<br>
#### 2、游戏操作<br>
使用WASD进行移动，鼠标移动控制枪支准星，鼠标左键进行开枪（右键瞄准是否实现暂定），ESC唤出游戏菜单实现退出和返回，（B键唤出商店进行枪支购买），Ctrl实现下蹲，Enter键唤出聊天框。<br>
### 三、游戏元素
#### 1、游戏玩家<br>
玩家分为人类和僵尸，暂定为网络下载模型。<br>
#### 2、游戏枪支<br>
枪支首先实现手枪和步枪，具体参数待定<br>
#### 3、游戏商店<br>
点击对应枪支图标进行购买<br>
#### 4、游戏地图<br>
先制作对战模式，CS1.6中的fy_iceworld雪地地图<br>
#### 5、游戏聊天框<br>
使用回车键唤出游戏聊天框并输入聊天内容，其将在游戏左下角显示。<br>
#### 6、计分板<br>
游戏画面顶部中间显示游戏计分板，顶部右侧显示击杀和阵亡数据<br>
### 四、游戏流程
开始游戏后连入服务器，进入游戏房间，购买枪支进行游戏<br>

## 界面设计

## 参考资料
[GameFramework Demo](https://github.com/mutouzdl/gameframework_demo)<br>
[基于GameFramework框架开发的游戏-StarForce](https://github.com/EllanJiang/StarForce)<br>
[笨木头与游戏开发](http://www.benmutou.com/archives/category/Game%20Framework)<br>
[Fast-Paced Multiplayer](https://www.gabrielgambetta.com/client-server-game-architecture.html)<br>
[What Every Programmer Needs To Know About Game Networking](https://gafferongames.com/post/what_every_programmer_needs_to_know_about_game_networking/)<br>
...
