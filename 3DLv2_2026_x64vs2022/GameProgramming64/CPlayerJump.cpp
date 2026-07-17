#include "CPlayerJump.h"
#include "CXCharacter.h"
#include "CCollider.h"

#define GRAVITY CVector(0.0f, -0.0312f, 0.0f) // 重力加速度
#define JUMP_V  CVector(0.0f, 0.6f, 0.0f)     // ジャンプ初速度

// 開始処理
void CPlayerJump::Start(CXCharacter* parent)
{
    // 親を保存
    mpParent = parent;

    // ジャンプ初速度
    mJumpV = JUMP_V;

    // ジャンプアニメーション
    mpParent->ChangeAnimation(7, false, 60);

    // 状態をジャンプにする
    mState = EState::EJUMP;
}

// 更新処理
void CPlayerJump::Update()
{
    // ジャンプ速度分だけ移動
    mpParent->Position(mpParent->Position() + mJumpV);

    // 重力を加える
    mJumpV = mJumpV + GRAVITY;
}

// 衝突処理
void CPlayerJump::Collision(CCollider* m, CCollider* o)
{
    switch (m->Type())
    {
    case CCollider::EType::ELINE:

        if (o->Type() == CCollider::EType::ETRIANGLE)
        {
            CVector adjust;

            if (CCollider::CollisionTriangleLine(o, m, &adjust))
            {
                // めり込み補正
                mpParent->Position(mpParent->Position() + adjust);

                // 待機状態へ
                mState = EState::EIDLE;
            }
        }

        break;
    }
}