TEMPLATE = subdirs
SUBDIRS = \
    tests

_qt_creator_ {
    SUBDIRS += src
}

OTHER_FILES = \
    # package/features/*.prf \
    include/* \
    qtautotrade.pri

QMAKE_CXXFLAGS += /MP
