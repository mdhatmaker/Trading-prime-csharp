#include "time2xtime.h"

/*
** SYSTEMTIMEתtime_t
*/

time_t systime_to_timet(const SYSTEMTIME& st)
{
	struct tm gm = { st.wSecond, st.wMinute, st.wHour, st.wDay, st.wMonth - 1, st.wYear - 1900, st.wDayOfWeek, 0, 0 };
	return mktime(&gm);
}
/*
**time_tתSYSTEMTIME
*/
SYSTEMTIME Time_tToSystemTime(time_t t)
{
	tm temptm = *localtime(&t);
	SYSTEMTIME st = { 1900 + temptm.tm_year,
		1 + temptm.tm_mon,
		temptm.tm_wday,
		temptm.tm_mday,
		temptm.tm_hour,
		temptm.tm_min,
		temptm.tm_sec,
		0 };
	return st;
}
/*
**time_tתSYSTEMTIME
*/
SYSTEMTIME TimetToSystemTime(time_t t)
{
	FILETIME ft;
	SYSTEMTIME pst;
	LONGLONG nLL = Int32x32To64(t, 10000000) + 116444736000000000;
	ft.dwLowDateTime = (DWORD)nLL;
	ft.dwHighDateTime = (DWORD)(nLL >> 32);
	FileTimeToSystemTime(&ft, &pst);
	return pst;
}
/*
**SYSTEMTIMEתtime_t
*/
time_t SystemTimeToTimet(SYSTEMTIME st)
{
	FILETIME ft;
	SystemTimeToFileTime(&st, &ft);
	LONGLONG nLL;
	ULARGE_INTEGER ui;
	ui.LowPart = ft.dwLowDateTime;
	ui.HighPart = ft.dwHighDateTime;
	nLL = (ft.dwHighDateTime << 32) + ft.dwLowDateTime;
	time_t pt = (long)((LONGLONG)(ui.QuadPart - 116444736000000000) / 10000000);
	return pt;
}
