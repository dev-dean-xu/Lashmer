import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { EmployeeService } from 'src/app/shared/services/employee.service';

@Component({
  selector: 'app-employee-promotion-codes',
  templateUrl: './employee-promotion-codes.component.html',
  styleUrls: ['./employee-promotion-codes.component.css']
})
export class EmployeePromotionCodesComponent implements OnInit {
	@Input() promotionCodes: string;
	@Input() userId: string;
	@Output() promotionCodesChanged = new EventEmitter();
	promotionCodeList: string[] = [];

	constructor(private modalService: NgbModal, private employeeService: EmployeeService) { }

	ngOnInit() {
		
	}

	openModal(promotionCodeModal) {
		if (this.promotionCodes != null && this.promotionCodes.length > 0) {
			this.promotionCodeList = this.promotionCodes.split(',');
		}

		this.modalService.open(promotionCodeModal, { ariaLabelledBy: 'Promotion Codes' });
	}

	onAddNewPromotionCode(code: string) {
		code = code.trim();
		if (code != null && code.length > 0) {
			this.employeeService.addEmployeePromotionCode(this.userId, code)
				.subscribe(
				() => {
					this.promotionCodeList.push(code);
					this.promotionCodesChanged.emit();
				},
					error => alert(error.error));
		}
	}

	onDeletePromotionCode(index, code) {
		code = code.trim();
		if (code != null && code.length > 0) {
			this.employeeService.deleteEmployeePromotionCode(this.userId, code)
				.subscribe(
					() => {
						this.promotionCodeList.splice(index, 1);
						this.promotionCodesChanged.emit();
					},
					error => alert(error.error));
		}
	}
}
