#ifndef CGAMESCENE_H
#define CGAMESCENE_H

#include "CSceneBase.h"
#include "CModel.h"
#include "CModelX.h"
#include "CXPlayer.h"
#include "CColliderMesh.h"

//ゲームシーン
class CGameScene : public CSceneBase
{
public:
    CGameScene();

    //シーン読み込み
    void Load();
    
    //シーン更新
    void Update();

private:
    CColliderMesh mColliderMesh; //メッシュコライダ
    CModel  mBackGround; //背景モデル
    CModelX mPlayer;//Xモデル
  

};

#endif