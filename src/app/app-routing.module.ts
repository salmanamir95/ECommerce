import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NavbarComponent } from './navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { ProductPageComponent } from './product-page/product-page.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { CartComponent } from './cart/cart.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  // {path: '', component: NavbarComponent}  ,
  {path: 'product-page/:name', component: ProductPageComponent},
  {path: 'product-detail/:name/:id', component: ProductDetailsComponent},
  {path: 'cart', component:CartComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
