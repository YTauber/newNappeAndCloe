import React, { Component } from 'react';
import axios from 'axios';
import Calendar from 'react-awesome-calendar';

export class Home extends Component {
  state = {
    events : [],
    date : ''
  }

  componentDidMount = () => {

    this.setState({date: new Date()}, () => {
      this.getJewishEvents();
    })

  }

  getJewishEvents = () => {

    const {date} = this.state;
    
    axios.get(`/api/calendar/getCalendarEvents/${date.getMonth() + 1}/${date.getFullYear()}`).then(({ data }) => {
            
      this.setState({events : data});
      
  });
  }

  onChange = (d) => {
      
      if (d.mode === "monthlyMode"){
        const date = new Date(d.year, d.month);
        this.setState({date}, () => {
          this.getJewishEvents();
        })
      }
      if (d.mode === "dailyMode"){
        const {month, day, year} = d;
        this.props.history.push(`/ViewDay/${month}/${day}/${year}`);
      }
  }

  render() {

    const {events} = this.state;
    const {onChange} = this;

    return (
      <div>
         <Calendar
                    events={events}
                    onChange={onChange}
                />
      </div>
    );
  }
}
