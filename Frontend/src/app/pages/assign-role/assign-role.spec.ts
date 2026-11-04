import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignRole } from './assign-role';

describe('AssignRole', () => {
  let component: AssignRole;
  let fixture: ComponentFixture<AssignRole>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignRole],
    }).compileComponents();

    fixture = TestBed.createComponent(AssignRole);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
