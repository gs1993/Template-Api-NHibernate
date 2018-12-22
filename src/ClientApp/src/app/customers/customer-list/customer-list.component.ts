import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../shared/customer.service';
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
    this.service.getCustomers();
  }

  populateForm(customer: Customer) {
    this.service.formData = new CustomerFormData(customer.Name, customer.Email, customer.Id);
  }

  delete(id: number) {
    this.service.deleteCustomer(id);
  }
}
