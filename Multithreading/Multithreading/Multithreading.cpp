#include <iostream>
#include <thread>
#include <vector>
#include <mutex>

using namespace std;

void print()
{
    std::cout << "Hello Thread\n";
    std::cout << "I'm here...\n";
    std::cout << "...not there.\n";
}

int main()
{
    vector<thread> threads;

    for (int i = 0; i < 50; ++i)
    {
        threads.push_back(thread(print));
    }

    for (auto& thread : threads)
    {
        thread.join();
    }

    return 0;
}


