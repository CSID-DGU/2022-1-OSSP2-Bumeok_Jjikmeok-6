public interface Life_Of_Basic
{
    void OnDie(); // 모든 생명은 죽기 마련

    void TakeDamage(int damage); // 목숨이 개수로 존재할 때 사용

    void TakeDamage(float damage); // 목숨이 게이지로 존재할 때 사용
}