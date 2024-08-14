import { Prop, SchemaFactory } from '@nestjs/mongoose';
import { Document } from 'mongoose';

export class Order extends Document {
  @Prop({ required: true, type: Number, default: 0 })
  totalPrice: number;

  @Prop({ required: true, type: Boolean, default: false })
  isPaid: boolean;

  @Prop({ required: true, type: Date })
  paidAt: Date;

  @Prop({ required: true, type: String })
  paymentMethod: string;
}

export const OrderSchema = SchemaFactory.createForClass(Order);
