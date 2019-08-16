import React, { Component } from 'react';
import axios from 'axios';
import Calendar from 'react-calendar';
import produce from 'immer';
import moment from 'moment';

export default class ViewDay extends Component {
    state = {
        date : '',
        day : {
            calendarEvents : [],
            orders : []
        },
    }

    componentDidMount = () => {

        const {year, month, day} = this.props.match.params;
        const date = new Date(year, month, day);
        this.setState({date}, () => {
            this.setDay()
        });
    }

    setDay = () => {
        const{date} = this.state;
        axios.get(`/api/calendar/getDay/${date.getMonth() + 1}/${date.getFullYear()}/${date.getDate()}`).then(({data}) => {
            this.setState({day : data})
        })
    }

    addDate = () => {
        const {date} = this.state;

        axios.post(`/api/draft/addDateDraft`, {date}).then(() => {

            this.props.history.push(`/newOrder`)
        })
    }

    viewCustomer = (id) => {
        this.props.history.push(`/customer/${id}`);
    }

    viewProduct = (id) => {
        this.props.history.push(`/viewProduct/${id}`)
    }

    viewOrder = (id) => {
        this.props.history.push(`/viewOrder/${id}`)
    }

    print = () => {
        const restorePage = document.body.innerHTML;
        const printContent = document.getElementById('printContent').innerHTML;
        document.body.innerHTML = printContent;
        window.print();
        window.location.reload();
        document.body.innerHTML = restorePage;
    }

    prev = () => {
        const nextState = produce(this.state, draft => {
            draft.date = moment(draft.date).subtract(1, 'days').toDate();
        });
        this.setState(nextState, () => {this.setDay()});
    }

    next = () => {
        const nextState = produce(this.state, draft => {
            draft.date = moment(draft.date).add(1, 'days').toDate();
        });
        this.setState(nextState, () => {this.setDay()});
    }

    changeDate = (date) => {
        console.log(date)
        this.setState({date},() => {this.setDay()})
    }

    render() {
        const {date, day} = this.state;
        const {calendarEvents, orders} = day;
        const {addDate, viewCustomer, viewProduct, print, prev, next, changeDate, viewOrder} = this;

        let ordersContent = ''
        if (orders.length){
            ordersContent = (
                <div>
                            <div className='col-md-12'>
                                <div className='col-md-6 col-md-offset-3'>
                                    <div style={{marginTop: 25}} className='col-md-12'>
                                        <button onClick={print} className='btn btn-block btn-sm btn-warning'>Print</button>
                                    </div>
                                    <div className='col-md-1' style={{marginTop: 15}}>
                                        <h4>{orders.length}</h4>
                                    </div>
                                </div>
                            </div>
                            <div id='printContent' className='row'>
                                <div className='col-md-12'>
                                    <div className='col-md-6 col-md-offset-3'>
                                    {orders.map((o) => 
                                        <div key={o.id} 
                                            className='row' 
                                            style={{margin: 15, border: '1px solid black', borderRadius: '5px', textAlign:'center'}}>
                                            <h4>
                                             <span style={{cursor : 'pointer'}} onClick={() => viewOrder(o.id)}>{o.name} </span>
                                                 for 
                                                <span style={{cursor : 'pointer'}} onClick={() => viewCustomer(o.customerId)}> {o.customer.name}</span>
                                            </h4>
                                            <hr />
                                            {o.productViews.map((p) => 
                                            <div key={p.id} className='col-md-12'>
                                                <div className="row well" onClick={() => viewProduct(p.id)} style={{cursor: 'pointer'}}>
                                                    <div className="" style={{marginLeft: 35, display:'inline', float:'left', margin: 5, textAlign: 'center', width: '30%'}}>
                                                            <img src={`/UploadedImages/${p.pictureName}`} alt="pic" className="img-responsive" style={{maxHeight: '75px', borderRadius: '50%'}} />
                                                    </div>
                                                    <div className='col-md-4' style={{margin: 5, textAlign: 'center'}}>
                                                    <label>{p.name}</label>
                                                        {p.productSizeViews.map((s) => <div key={s.id}>
                                                            <h5>{s.quantity} {s.size}</h5>
                                                        </div>)}
                                                    </div>
                                                </div>
                                            </div>)}
                                        </div>)}
                                    </div>
                                </div>
                            </div>
                            <div className='row'>
                       </div>
                </div>
            )
        }
        return (
            <div>
               <div className='col-md-3' style={{marginTop: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                <div className='row'>
                    <div className='col-md-6 col-md-offset-3'>
                              <div style={{textAlign:'center'}} className='col-md-12'>
                                <div className='col-md-6' style={{display:'inline', float:'left'}}>
                                    <button onClick={prev} className='btn btn-link'>Prev Day</button>
                                </div>
                                <div className='col-md-6'>
                                    <button onClick={next} className='btn btn-link'>Next Day</button>
                                </div>
                            </div>
                        <div style={{marginTop: 25}} className='col-md-12'>
                            
                            <div className='col-md-offset-2'>
                                <div style={{width: '100%'}}>
                                    <Calendar
                                        value={date}
                                        calendarType="US"
                                        onChange={changeDate}
                                    />
                                </div>
                            </div>
                        </div>
                        <div>
                            {calendarEvents.map((e) => <button className='btn btn-lg btn-block' style={{color: e.color}}>{e.title}</button>)}
                        </div>
                    </div>
                        {ordersContent}
                    <div className='col-md-6 col-md-offset-3'>
                        <div style={{marginTop: 25}} className='col-md-12'>
                            <button className='btn btn-success btn-block' onClick={addDate}>Add to order</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
