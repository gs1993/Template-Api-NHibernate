import { Injectable } from '@angular/core';
import { CustomerFormData, Customer } from './customer.model';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../config/api-response';
import { ToastrService } from '../../../../node_modules/ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  formData: CustomerFormData;
  list: Customer[];
  readonly apiUrl: string  = 'http://localhost:60367/api/Customers/';

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  postCustomer(customer: CustomerFormData) {
    return this.http.post(this.apiUrl, customer);
  }

  updateCustomer(customer: CustomerFormData) {
    return this.http.put(this.apiUrl + customer.Id, customer);
  }

  getCustomers() {
    return this.http.get(this.apiUrl)
    .toPromise().then((response: ApiResponse) => {
      this.handleResponse(response);
    });
  }

  handleResponse(response: ApiResponse) {
    if (response.errorMessage != null) {
      this.toastr.error(response.errorMessage, 'Error');
      return;
    }

    this.list = this.parseToCustomer(response.result);
  }

  parseToCustomer(data: Object[]) {
    const list = [];
    data.forEach(element => {
      list.push(new Customer((element)));
    });

    return list;
  }

  populateForm(customer: Customer) {
    this.formData = new CustomerFormData(customer.Name, customer.Email, customer.Id);
  }

  deleteCustomer(id: Number) {
    this.http.delete(this.apiUrl + id)
    .toPromise().then((response: ApiResponse) => {
      this.getCustomers();
      this.toastr.success('The customer has been deleted');
    }, msg => {
      console.log(msg);
    });
  }
}
