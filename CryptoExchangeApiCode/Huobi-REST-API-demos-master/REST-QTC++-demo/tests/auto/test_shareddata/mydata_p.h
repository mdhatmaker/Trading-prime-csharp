#ifndef MYDATA_P_H
#define MYDATA_P_H

#include <QSharedData>

class MyDataPrivate : public QSharedData
{
public:
    MyDataPrivate()
        : id(-1)
    {}

    MyDataPrivate(const MyDataPrivate& other)
        : QSharedData(other)
        , id(other.id)
    {}
    ~MyDataPrivate()
    {}

    qint64 id;
};

#endif // MYDATA_P_H
