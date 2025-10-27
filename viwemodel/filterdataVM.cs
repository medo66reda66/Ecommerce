namespace Ecommerce.viwemodel
{
    public record filterdataVM(
        string name, decimal? minprice, decimal? maxprice, int? catecoryid, int? brandid, bool hot,bool lessQuantity
        );
    
    
}
