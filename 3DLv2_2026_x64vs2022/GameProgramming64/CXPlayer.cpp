#include "CXPlayer.h"

#define GRAVITY 0.0625f // 重力

void CXPlayer::Update()
{
    // GRAVITYの大きさだけ、下方向へ移動させる
    CVector gravity(0.0f, -GRAVITY, 0.0f);
    Position(Position() + gravity);

    // 親クラスの更新
    CXCharacter::Update();
}