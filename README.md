# Project-FF
Unity 2022.3.62f2

프로젝트 소개: 간단하게 물고기를 키운뒤 그 물고기를 바탕으로 목표에 도달하는 게임.

사용한 것들? : 어드레서블, 풀링 기법, 테이블, 번역, 로컬 저장

프로젝트 구조 : 씬은 2개 Title, Game 이렇게 한 이유는 굳이 나눌 필요가 없어 보였기 때문. 굳이 나눌 필요가 없을 정도는 간단한 게임이기 떄문에 이렇게 작업. 만약 씬을 나누게 된다면 Title, Lobby, Aqua, Race 이렇게 나눴을듯?

로비 - 어항? - 레이스 반복 구조

로비 : UI_Lobby가 메인 

어항 : AquaMgr가 메인

레이스 : RaceMgr가 메인

로비에서는 SceneGame에서 Pooling 기법을
어항에서는 AquaMgr에서 Pooling 기법을
레이스에서는 RaceMgr에서 Pooling 기법을 사용함. 
