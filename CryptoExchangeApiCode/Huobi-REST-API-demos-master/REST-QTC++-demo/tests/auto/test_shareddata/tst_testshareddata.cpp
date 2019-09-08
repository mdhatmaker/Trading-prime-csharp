#include <QString>
#include <QtTest>
#include "mydata.h"

class TestSharedData : public QObject
{
    Q_OBJECT

public:
    TestSharedData();

private Q_SLOTS:
    void testCase1();
};

TestSharedData::TestSharedData()
{
}

void TestSharedData::testCase1()
{
    MyData d1 , d2;

    d1 = d2;
}


QTEST_APPLESS_MAIN(TestSharedData)

#include "tst_testshareddata.moc"
