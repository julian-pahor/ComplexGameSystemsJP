#pragma once
#include <memory>
#include <stdatomic.h>

class circular_queue
{
public:
	circular_queue();
	~circular_queue();

	bool push(const int& item);
	bool pop(int& item);

	bool is_empty() const;
	bool is_full() const;

private:
	size_t increment(size_t index) const;

};

