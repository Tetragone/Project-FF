Project-FF
Unity 2022.3.62f2

 1. 프로젝트 소개
간단하게 물고기를 키운뒤 그 물고기를 바탕으로 목표에 도달하는 게임.

 2. 사용한 기술
어드레서블, 풀링 기법, 테이블, 번역, 로컬 저장, 아틀라스

  ㄱ. 어드레서블
   PopupMgr.cs, PoolData.cs에서 사용.
   팝업을 불러오거나, Object Pool을 위한 리소스를 불러올때 사용함. 

   Resources.Load를 사용하면 편하나, Addressable을 사용한 적이 없었으며, 최적화를 위하면 사용하는 편이 좋다고 판단함.

   코드 예시


  ㄴ. 풀링 기법
   PoolData.cs에서 구현 및 SceneGame.cs, AquaMgr.cs, RaceMgr.cs에서 사용.
   물고기를 생성할때나, 먹이를 생성할때처럼 많은 Object를 생성해야 할 필요가 있는 경우 사용을 함.
   다만 미리 생성하는 구조는 아니고 필요할때 생성하지만 생성된 오브젝트를 저장하는 개념으로 사용함.

   처음에 미리 생성하지 않은 이유는 한번에 많은 오브젝트를 생성하지 않지만 시간이 지나면서 많은 오브젝트를 생성할 가능성이 있기 때문에 이렇게 사용함.
 
   코드 예시
 

  ㄷ. 테이블
   TableMgr.cs에서 구현 및 많은 부분에서 사용.

   코드 예시

  ㄹ. 번역
   TransMgr.cs에서 구현 및 많은 부분에서 사용.
   Unity의 Localization을 사용하여 구현.
   직접 번역 관련된 것을 구현할 수 있었으나, Unity의 Localization을 사용해 봄으로써 어떤 점이 더 좋은지 파악하기 위하여 사용함. 

   코드 예시

  ㅁ. 로컬 저장
   UpgradeMgr.cs, UserDataMgr.cs에서 사용.
   Unity의 PlayerPrefs를 사용하여 저장.
   서버 저장과 로컬 저장 중에 선택할 수 있도록 선택권을 주기 위하여 사용하였으며, 간단한 게임인 만큼 간단하게 저장하기 위해서 사용.

   코드 예시

  ㅂ. 아틀라스
   AtlasMgr.cs에서 사용.
   간단한 게임이나, draw call관리는 중요해서 사용. 

   코드 예시 및 사용 예시

 3. 프로젝트 구조
씬은 Title.scene, Game.scene 2개를 사용.
간단한 게임이기 때문에 씬을 좀 더 세분화할 필요는 없다고 생각해서 위와 같이 작업함.
만약 씬을 좀 더 세분화할 필요가 있었다면 Title.scene, Lobby.scene, Aqua.scene, Race.scene을 사용했을 것임.

 Title.scene
데이터 로딩을 위하여 사용, 만약 서버가 있었다면 여기서 서버 연결을 했을 것.

 Game.scene
로비, 어항, 레이스에서 사용.

  ㄱ. 로비
  <img width="3164" height="1324" alt="image" src="https://github.com/user-attachments/assets/b5a71089-07b1-446f-abf5-f61200e78bc0" />

  ㄴ. 어항
  
  ㄷ. 레이스


