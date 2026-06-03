//背景モデルデータの指定
#include "CTaskManager.h"
#include "CGmeScene.h"
#include "CCharacter3.h"
#include"CXCharacter.h"
#include "CXPlayer.h"
#define MODEL_BACKGROUND "res\\sky.obj", "res\\sky.mtl"
CGameScene::CGameScene()
	: CSceneBase(EScene::eGame)
{
}
void CGameScene::Load()
{
    //課題 背景モデルデータの読み込み
    mBackGround.Load(MODEL_BACKGROUND);

    //キャラクタのインスタンス作成
    CCharacter3* character = new CCharacter3();

    //キャラクタのモデルの設定
    character->Model(&mBackGround);
    mPlayer.Load(MODEL_FILE);
	//X プレイヤー
	CXPlayer* player = new CXPlayer();

	//設定
	player->Init(&mPlayer);

	//位置
	player->Position(CVector(1.0f, 0.0f, 0.0f));
}
void CGameScene::Update()
{
	//カメラの設定
	gluLookAt(1.0f, 2.0f, 10.0f,
		0.0f, 2.0f, 0.0f,
		0.0f, 1.0f, 0.0f);
	//全キャラクタの更新
	CTaskManager::Instance()->Update();
	//課題 全キャラクタの描画
	CTaskManager::Instance()->Render();
}