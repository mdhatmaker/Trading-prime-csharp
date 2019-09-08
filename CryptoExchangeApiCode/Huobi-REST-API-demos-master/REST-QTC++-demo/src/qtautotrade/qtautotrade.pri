QT += core network
CONFIG += c++11
INCLUDEPATH += $$PWD

HEADERS += \
    $$PWD/core/appmarketaccount.h \
    $$PWD/core/baseentity.h \
    $$PWD/core/asset.h \
    $$PWD/core/abstractmarketapi.h \
    $$PWD/utils/httputils.h \
    $$PWD/core/bitorder.h \
    $$PWD/core/kline.h \
    $$PWD/core/marketparam.h \
    $$PWD/core/ticker.h \
    $$PWD/core/depth.h \
    $$PWD/utils/timerouthelper.h \
    $$PWD/utils/httpresponse.h \
    $$PWD/common/appconfig.h \
    $$PWD/common/cacheutils.h \
    $$PWD/core/marketapifactory.h \
    $$PWD/market/huobi/huobproapi.h

SOURCES += \
    $$PWD/core/appmarketaccount.cpp \
    $$PWD/core/baseentity.cpp \
    $$PWD/core/asset.cpp \
    $$PWD/core/abstractmarketapi.cpp \
    $$PWD/utils/httputils.cpp \
    $$PWD/core/bitorder.cpp \
    $$PWD/core/kline.cpp \
    $$PWD/core/marketparam.cpp \
    $$PWD/core/ticker.cpp \
    $$PWD/core/depth.cpp \
    $$PWD/utils/timerouthelper.cpp \
    $$PWD/utils/httpresponse.cpp \
    $$PWD/common/appconfig.cpp \
    $$PWD/common/cacheutils.cpp \
    $$PWD/core/marketapifactory.cpp \
    $$PWD/market/huobi/huobproapi.cpp

include(../../vendor/qtpromise/qtpromise.pri)

