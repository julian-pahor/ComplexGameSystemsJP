#include <iostream>
#include <thread>
#include <vector>
#include <mutex>
#include <string>
#include <glm/glm.hpp>

#include "circular_queue.h"

using namespace glm;
using namespace std;

void push_function(circular_queue& a_circular_queue);
void pop_function(circular_queue& a_circular_queue);

const int MAX_VALUE = 50;

void push_function(circular_queue& a_circular_queue)
{
	int count = 0;
	while (count <= MAX_VALUE)
	{
		if (a_circular_queue.push(count))
		{
			cout << "Pushed " << count << " to the queue " << endl;
			count++;
		}

		this_thread::sleep_for(chrono::seconds(1));
	}
}

void pop_function(circular_queue& a_circular_queue)
{
	int value = 0;
	while (value < MAX_VALUE)
	{
		if (a_circular_queue.pop(value))
		{
			cout << "Popped " << value << " from the queue " << endl;
			value++;
		}
		this_thread::sleep_for(chrono::seconds(2));
	}
}

int main()
{
	circular_queue m_circular_queue;

	auto publisher = thread(push_function, ref(m_circular_queue));

	auto consumer = thread(pop_function, ref(m_circular_queue));

	publisher.join();
	consumer.join();
	return 0;
}

//////
//Normalising multiple vec4's using multiple threads to speed up the process


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

//int main()
//{
//    vector<thread> threads;
//    vec4 myVectors[50000] = { };
//    int chunkLength = 50000 / 10;
//    //mutex myMutex;
//
//    //for (int i = 0; i < 50; ++i)
//    //{
//    //    threads.push_back(thread(
//    //        [&myMutex](int i) 
//    //        {
//    //            lock_guard<mutex> guard(myMutex);
//    //            cout << "Hello Thread_" << i << "\n";
//    //            cout << "I'm here...\n";
//    //            cout << "...not there\n";
//    //        }, i
//    //    ));
//    //}
//
//    for (int i = 0; i < 10; i++)
//    {
//        threads.push_back(thread([&](int low, int high) {
//            for (int j = low; j < high; j++)
//            {
//                myVectors[j] = normalize(myVectors[j]);
//            }
//            }, i * chunkLength, (i + 1) * chunkLength));
//    }
//
//    for (auto& thread : threads)
//    {
//        thread.join();
//    }
//
//    return 0;
//}


