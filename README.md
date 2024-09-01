# Project-FF
Unity 2022.3.25f1


미완성 프로젝트입니다. 

코드 스타일이 잘 보일것 같은 Commit입니다. 

코드 스타일 1 https://github.com/Tetragone/Project-FF/commit/aaf12f0b561d488d4081a040f33546c3f02577ea

코드 스타일 2 https://github.com/Tetragone/Project-FF/commit/3ec95ce3ef9145fb46c6bed7a385ebd39654065d

코드 스타일 3 https://github.com/Tetragone/Project-FF/commit/3d270fd87a252a9a512c658bf222004d22ac4e70

코드 스타일 4 https://github.com/Tetragone/Project-FF/commit/ba559eeeee9f4a5fea7f917e950c60b603937f83

코딩 중에 생각했던 점.

1. 구조는 어떻게 할것인가.

   AquaMgr와 RaceMgr라는 두개의 Mgr를 가지고 각각 AquaFish와 RaceFish를 생성하여 객체를 제어하는 방식을 사용.
   두 객체는 하나의 FishData로 값을 가져올 수 있도록 생성. 
   
2. 물고기의 구조. 
   일단 데이터는 공유를 한다. (어떤 먹이를 먹었는지, 아니면 얼마만큼 자랐는지. + 스탯등등)
   하지만 물고기들은 각 게임 모드마다 다른 방식으로 제어가 필요하다. 
   즉. 데이터는 공유하되, 각자 새로운 클래스에서 제어가 필요. 
   
   FishData라는 클래쓰를 제작함.

3. 마우스 클릭은 어디서 제어할 것인가?
   aquaMgr? 아쿠아 매니저가 맞는 선택인가? 입력은 터치 입력이 있음. 어찌보면 UI에서 오는 입력. 그렇다면 UI에서 따로 하는것이 맞음. 아쿠아 매니저는 아님. 

   관련 Commit https://github.com/Tetragone/Project-FF/commit/ba559eeeee9f4a5fea7f917e950c60b603937f83 (코드 스타일 4와 동일한 Commit)

4. Object 관리
   생성은 그냥 그 자리에서 하는 방식으로 하고, 다만 생성 후 재사용이 가능하도록 수정이 필요함. 
   그렇게 하기 위해서 poolData를 생성하고 poolData에 데이터를 추가하고 싶을때는 그냥 거기서 getnew하면 되는거 아님?

   관련 Commit https://github.com/Tetragone/Project-FF/commit/04364ca41042be204f4cdfbb2693b793581f82c8

5. 보안 관련
   Secure라는 클래스를 만들고 그 밑에 SecureFloat같은거 만들어서 하는 편이 좋을것이라고 생각.

   관련 Commit https://github.com/Tetragone/Project-FF/commit/0aa561565995b2c81610f93bd057055aeb05ac7a

6. UI_Top을 만들까?라는 의문
   장점 : 뭐 따로 있는거니까 관리 측면에서 좋지
   단점 : 뭐.. 굳이?
   안 만들어서 관련 Commit 없습니다. 
