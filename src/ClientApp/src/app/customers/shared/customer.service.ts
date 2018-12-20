import { Injectable } from '@angular/core';
import { CustomerFormData, Customer } from './customer.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  formData: CustomerFormData;
  list: Customer[];
  readonly apiUrl: string  = 'http://localhost:60367/api/Customers/';

  constructor(private http: HttpClient) { }

  postCustomer(customer: CustomerFormData) {
    return this.http.post(this.apiUrl, customer);
  }

  getCustomers() {
    return this.http.get(this.apiUrl);
  }
}
