import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shared/models/product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product?: Product;
  quantity = 1;
  quantityInBasket = 0;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute, 
    private bcService: BreadcrumbService, private basketService: BasketService) {
      this.bcService.set('@productDetails', ' ')
    }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) this.shopService.getProduct(+id).subscribe({
      next: product => {
        this.product = product;
        this.bcService.set('@productDetails', product.name);
        //take 1 = only takes 1 value from the basket source observable
        this.basketService.basketSource$.pipe(take(1)).subscribe({
          next: basket => {
            //checks to see if we have the selected item in the basket currently
            const item = basket?.items.find(x => x.id === +id);
            //if we have the item in the basket then update the quantities
            if (item) {
              this.quantity = item.quantity;
              this.quantityInBasket = item.quantity;
            }
          }
        })
      },
      error: error => console.log(error)
    })
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    this.quantity--;
  }

  updateBasket() {
    if (this.product) {
      //if the quantity is bigger than the quantity in the basket it means we have items needed to be added
      if (this.quantity > this.quantityInBasket) {
        //value of number of items to be added to the basket of this product
        const itemsToAdd = this.quantity - this.quantityInBasket;
        //update the quantity in basket amount before calling the service
        this.quantityInBasket += itemsToAdd;
        //add the product and the quantity
        this.basketService.addItemToBasket(this.product, itemsToAdd);
      } else {
        //if we're removing items
        const itemsToRemove = this.quantityInBasket - this.quantity;
        this.quantityInBasket -= itemsToRemove;
        this.basketService.removeItemFromBasket(this.product.id, itemsToRemove);
      }
    }
  }

  get buttonText() {
    return this.quantityInBasket === 0 ? 'Add to basket' : 'Update basket';
  }

}