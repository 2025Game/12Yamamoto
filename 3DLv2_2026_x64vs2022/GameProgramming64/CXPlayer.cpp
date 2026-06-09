#include "CXPlayer.h"
#include "CCollisionManager.h"

#define GRAVITY 0.0625f 

CXPlayer::CXPlayer()
{
    mColliderLine.Set(
        this,
        &mMatrix,
        CVector(0.0f, 0.0f, 0.0f),
        CVector(0.0f, 3.5f, 0.0f)
    );
}

// 重力
void CXPlayer::Update()
{
    // GRAVITYの大きさだけ、下方向へ移動させる
    CVector gravity(0.0f, -GRAVITY, 0.0f);
    Position(Position() + gravity);

    // 親クラスの更新
    CXCharacter::Update();
}

void CXPlayer::Collision(CCollider* m, CCollider* o)
{
    //自身のコライダタイプの判定
    switch (m->Type()) {
    case CCollider::EType::ELINE://線分コライダ
        //相手のコライダが三角コライダの時
        if (o->Type() == CCollider::EType::ETRIANGLE)
        {
            CVector adjust;//調整用ベクトル
            //三角形と線分の衝突判定
            if (CCollider::CollisionTriangleLine(
                o, m, &adjust))
            {
                //位置の更新(mPosition + adjust)
                mPosition = mPosition + adjust;
                //行列の更新
                CTransform::Update();
            }
        }
        break;
    }
}
void CXPlayer::Collision()
{
    //コライダの優先度変更
    mColliderLine.ChangePriority();
    //衝突処理を実行
    CCollisionManager::Instance()->Collision(
        &mColliderLine, COLLISIONRANGE);
}
