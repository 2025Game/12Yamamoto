#include "CApplication3.h"
#include "CGmeScene.h"

CApplication3::CApplication3()
{
}

CApplication3::~CApplication3()
{
}

void CApplication3::Start()
{
    //ゲームシーンのインスタンスを作成
    mpScene = std::make_unique<CGameScene>();

    //ゲームシーンのロード
    mpScene->Load();
}

void CApplication3::Update()
{
    //ゲームシーンの更新
    mpScene->Update();
}