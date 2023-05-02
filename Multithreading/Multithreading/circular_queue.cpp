#include "circular_queue.h"

circular_queue::circular_queue() : m_read_index(0), m_write_index(0)
{

}

bool circular_queue::push(const int& item)
{
	if (is_full())
	{
		return false;
	}

	auto current_index = m_write_index.load();
	auto next_index = increment(current_index);
	m_data[current_index] = item;
	m_write_index.store(next_index);
	return true;
}

bool circular_queue::pop(int& item)
{
	if (is_empty())
	{
		return false;
	}

	auto current_index = m_read_index.load();
	item = m_data[current_index];
	m_read_index.store(increment(current_index));
	return true;
}

bool circular_queue::is_empty() const
{
	return m_read_index.load() == m_write_index.load();
}

bool circular_queue::is_full() const
{
	const auto next_write_index = increment(m_write_index.load());
	return (next_write_index == m_read_index.load());
}

size_t circular_queue::increment(size_t index) const
{
	return (index + 1) % SIZE;
}
