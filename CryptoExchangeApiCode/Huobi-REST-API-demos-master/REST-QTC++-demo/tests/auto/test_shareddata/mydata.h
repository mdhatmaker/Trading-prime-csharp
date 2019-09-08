#ifndef MYDATA_H
#define MYDATA_H

#include <QtCore/QSharedDataPointer>

class MyDataPrivate;
class MyData
{
public:
    MyData();
    MyData(const MyData& other);
    MyData& operator=(const MyData& other);
    ~MyData();
    qint64 getId() const;
    void setId(qint64 id);

private:
    QSharedDataPointer<MyDataPrivate> d;
};

#endif // MYDATA_H
