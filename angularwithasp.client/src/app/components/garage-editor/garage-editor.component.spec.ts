import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GarageEditorComponent } from './garage-editor.component';

describe('GarageEditorComponent', () => {
  let component: GarageEditorComponent;
  let fixture: ComponentFixture<GarageEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GarageEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GarageEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
