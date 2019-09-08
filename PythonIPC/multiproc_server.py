# The multiprocessing library provides listeners and clients that wrap
# sockets and allow you to pass arbitrary python objects.
from multiprocessing.connection import Listener

address = ('localhost', 6000)     # family is deduced to be 'AF_INET'
listener = Listener(address, authkey='secret password')
conn = listener.accept()
print 'connection accepted from', listener.last_accepted
while True:
    msg = conn.recv()
    # do something with msg
    if msg == 'close':
        conn.close()
        break
listener.close()
