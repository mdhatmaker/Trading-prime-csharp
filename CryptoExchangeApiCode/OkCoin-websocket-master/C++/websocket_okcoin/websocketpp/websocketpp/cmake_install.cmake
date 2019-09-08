# Install script for directory: D:/websocketpp-master/websocketpp-master/websocketpp

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "D:/websocketpp-master/websocketpp-master/install")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/base64" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/base64/base64.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/client.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/close.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/asio.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/asio_ssl.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/chrono.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/connection_hdl.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/cpp11.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/functional.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/md5.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/memory.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/network.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/platforms.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/random.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/regex.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/stdint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/system_error.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/thread.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/time.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/common" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/common/type_traits.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/concurrency" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/concurrency/basic.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/concurrency" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/concurrency/none.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/asio.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/asio_client.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/asio_no_tls.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/asio_no_tls_client.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/boost_config.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/core.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/core_client.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/debug.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/debug_asio.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/debug_asio_no_tls.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/minimal_client.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/config" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/config/minimal_server.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/connection.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/connection_base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/endpoint_base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/error.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/extensions" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/extensions/extension.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/extensions/permessage_deflate" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/extensions/permessage_deflate/disabled.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/extensions/permessage_deflate" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/extensions/permessage_deflate/enabled.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/frame.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/constants.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http/impl" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/impl/parser.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http/impl" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/impl/request.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http/impl" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/impl/response.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/parser.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/request.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/http" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/http/response.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/impl" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/impl/connection_impl.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/impl" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/impl/endpoint_impl.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/impl" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/impl/utilities_impl.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/logger" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/logger/basic.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/logger" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/logger/levels.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/logger" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/logger/stub.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/logger" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/logger/syslog.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/message_buffer" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/message_buffer/alloc.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/message_buffer" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/message_buffer/message.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/message_buffer" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/message_buffer/pool.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/processors" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/processors/base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/processors" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/processors/hybi00.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/processors" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/processors/hybi07.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/processors" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/processors/hybi08.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/processors" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/processors/hybi13.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/processors" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/processors/processor.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/random" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/random/none.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/random" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/random/random_device.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/roles" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/roles/client_endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/roles" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/roles/server_endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/server.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/sha1" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/sha1/sha1.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/asio" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/asio/base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/asio" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/asio/connection.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/asio" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/asio/endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/asio/security" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/asio/security/base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/asio/security" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/asio/security/none.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/asio/security" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/asio/security/tls.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/base" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/base/connection.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/base" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/base/endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/debug" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/debug/base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/debug" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/debug/connection.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/debug" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/debug/endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/iostream" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/iostream/base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/iostream" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/iostream/connection.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/iostream" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/iostream/endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/stub" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/stub/base.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/stub" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/stub/connection.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp/transport/stub" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/transport/stub/endpoint.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/uri.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/utf8_validator.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/utilities.hpp")
endif()

if(NOT CMAKE_INSTALL_COMPONENT OR "${CMAKE_INSTALL_COMPONENT}" STREQUAL "Unspecified")
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/websocketpp" TYPE FILE FILES "D:/websocketpp-master/websocketpp-master/websocketpp/version.hpp")
endif()

