using System;
namespace IQ_Config_Namespace
{
public class IQ_Config
{
string most_recent_protocol = "5.2";
string customer_product_id = "MICHAEL_HATMAKER_12300";
public IQ_Config()
{
}
public string getProtocol()
{
return most_recent_protocol;
}
public string getProductID()
{
return customer_product_id;}
}
}