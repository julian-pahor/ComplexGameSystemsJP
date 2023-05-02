#pragma once
#include <atomic>


class circular_queue
{
public:
	circular_queue();
	~circular_queue() {};

	bool push(const int& item);
	bool pop(int& item);

	bool is_empty() const;
	bool is_full() const;

private:
	size_t increment(size_t index) const;

	std::atomic<size_t> m_read_index;
	std::atomic<size_t> m_write_index;

	static const int SIZE = 11;
	int m_data[SIZE];
};

