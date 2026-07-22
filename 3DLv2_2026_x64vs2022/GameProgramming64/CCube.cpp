#include "CCube.h"

//モデルデータの指定
#define MODEL_CUBE "res\\cube.obj", "res\\cube.mtl"

//静的メンバ変数の定義
CModel CCube::msModel;

CCube::CCube()
{
    //モデルデータがなければ読み込み
    if (msModel.Triangles().empty()) {
        //課題 モデルデータを読み込み
        msModel.Load(MODEL_CUBE);
    }

    //モデルポインタの設定
    mpModel = &msModel;

    //上面コライダ
    mCollider[0].Set(
        this,
        &mMatrix,
        CVector(-0.5f, 0.5f, -0.5f),
        CVector(0.5f, 0.5f, -0.5f),
        CVector(0.5f, 0.5f, 0.5f)
    );

    mCollider[1].Set(
        this,
        &mMatrix,
        CVector(-0.5f, 0.5f, -0.5f),
        CVector(0.5f, 0.5f, 0.5f),
        CVector(-0.5f, 0.5f, 0.5f)
    );
}

void CCube::Update()
{
   mRotation.Y(mRotation.Y() + 1.0f);
    CTransform::Update();
}
