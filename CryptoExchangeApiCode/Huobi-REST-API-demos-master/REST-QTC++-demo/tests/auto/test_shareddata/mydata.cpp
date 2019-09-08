#include "mydata.h"
#include "mydata_p.h"

MyData::MyData()
    : d(new MyDataPrivate)
{
}

MyData::MyData(const MyData &other)
    : d(other.d)
{}

MyData& MyData::operator=(const MyData &other)
{
    d = other.d;
    return *this;
}

MyData::~MyData(){}

qint64 MyData::getId() const
{
    return d->id;
}

void MyData::setId(qint64 id)
{
    d->id = id;
}
