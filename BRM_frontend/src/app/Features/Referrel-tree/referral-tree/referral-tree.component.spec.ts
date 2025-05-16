import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferralTreeComponent } from './referral-tree.component';

describe('ReferralTreeComponent', () => {
  let component: ReferralTreeComponent;
  let fixture: ComponentFixture<ReferralTreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReferralTreeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReferralTreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
