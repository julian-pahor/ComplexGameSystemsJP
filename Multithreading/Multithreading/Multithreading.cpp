#include <iostream>
#include <thread>
#include <vector>
#include <mutex>
#include <string>
#include <glm/glm.hpp>

using namespace glm;
using namespace std;

//void print(int i)
//{
//    static mutex myMutex;
//
//    lock_guard<mutex> guard(myMutex);
//
//    std::cout << "Hello Thread_" << i << "\n";
//    std::cout << "I'm here...\n";
//    std::cout << "...not there.\n";
//}

int main()
{
    vector<thread> threads;
    vec4 myVectors[50000] = { };
    int chunkLength = 50000 / 10;
    //mutex myMutex;

    //for (int i = 0; i < 50; ++i)
    //{
    //    threads.push_back(thread(
    //        [&myMutex](int i) 
    //        {
    //            lock_guard<mutex> guard(myMutex);
    //            cout << "Hello Thread_" << i << "\n";
    //            cout << "I'm here...\n";
    //            cout << "...not there\n";
    //        }, i
    //    ));
    //}

    for (int i = 0; i < 10; i++)
    {
        threads.push_back(thread([&](int low, int high) {
            for (int j = low; j < high; j++)
            {
                myVectors[j] = normalize(myVectors[j]);
            }
            }, i * chunkLength, (i + 1) * chunkLength));
    }

    for (auto& thread : threads)
    {
        thread.join();
    }

    return 0;
}


