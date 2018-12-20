import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../shared/customer.service';
import { ApiResponse } from '../../config/api-response';
import { ToastrService } from '../../../../node_modules/ngx-toastr';
import { Customer, CustomerFormData } from '../shared/customer.model';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {

  constructor(private service: CustomerService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.getCustomers()
    .subscribe((response: ApiResponse) => {
      this.handleResponse(response);
    });
  }

  handleResponse(response: ApiResponse) {
    if (response.errorMessage != null) {
      this.toastr.error(response.errorMessage, 'Error');
      return;
    }

    this.service.list = this.parseToCustomer(response.result);
  }

  parseToCustomer(data: Object[]) {
    const list = [];
    data.forEach(element => {
      list.push(new Customer((element)));
    });

    return list;
  }

  populateForm(customer: Customer) {
    this.service.formData = new CustomerFormData(customer.Name, customer.Email);
  }
}
