import React, { Component } from 'react';
import {produce} from 'immer';
import axios from 'axios';
import ReactTags from 'react-tag-autocomplete';
import NumberFormat from 'react-number-format';

export default class AddProduct extends Component {
    state = {
        product : {
            name : '',
            price : '',
            notes : '',
            pictureName: 'Default.jpg'
        },
        sizes:[],
        tags : [],
        suggestions : [],
        message: '',
        
    }

    componentDidMount = () => {

        axios.get('/api/product/getalllabels').then(({ data }) => {

            const nextState = produce(this.state, draft => {
                draft.suggestions = data;
                draft.sizes.push({})
            });
            this.setState(nextState);

        });
    }

    onInputChange = e => {
        const nextState = produce(this.state, draft => {
            draft.product[e.target.name] = e.target.value;
        });
        this.setState(nextState);
    }

    onAddClick = () => {

        const {name, price, notes, pictureName} = this.state.product;
        let {tags, sizes} = this.state;
        tags = tags.map(t => t.name)

        if (!name){
            this.setState({message : 'You didn\'t enter a valid name... :('})
        }

        
        else if (!price || isNaN(price)){
            this.setState({message : 'You didn\'t enter a valid price... :('})
        }

        else if (sizes.some(s => !s.size) || sizes.some(s => !s.quantity)){

            this.setState({message : 'Please fill out all the sizes...'})
        }
       

        else {

        axios.post( '/api/product/addproduct', {name, price, notes, pictureName}).then(({data}) => {

                const productId = data.id;
                axios.post('/api/product/addlabels', {tags, productId}).then(() => {

                    axios.post('/api/product/addSizes', {sizes, productId}).then(() => {
                
                        this.props.history.push('/Inventory');
                    });
                });
            });

        }
       
    }

    handleDelete = (i) => {
        const tags = this.state.tags.slice(0)
        tags.splice(i, 1)
        this.setState({ tags })
      }
     
      handleAddition = (tag) => {
        const tags = [].concat(this.state.tags, tag)
          this.setState({ tags })
      }

      onFileChange = (e) => {

        const formData = new FormData();
        formData.append('file', e.target.files[0])

        axios.post('api/product/addpicture' , formData).then(({data}) => {
            
            const nextState = produce(this.state, draft => {
                draft.product.pictureName = data
            });
            this.setState(nextState);

        })
        
          
      }

      addSize = () => {

        const nextState = produce(this.state, draft => {
            draft.sizes.push({})
        });
        this.setState(nextState);
      }

      removeSize = (idx) => {

        const nextState = produce(this.state, draft => {
            draft.sizes.splice(idx, 1);
        });
        this.setState(nextState);
      }

      changeSize = (e, idx) => {

        const nextState = produce(this.state, draft => {
            draft.sizes[idx][e.target.name] = e.target.value
        });
        this.setState(nextState);
      }
     
    render() {
        const {product, tags, suggestions, message, sizes} = this.state;
        const {onInputChange, onAddClick, handleAddition, handleDelete, onFileChange, addSize, removeSize, changeSize} = this;
        const {name, price, notes, pictureName} = product;

        let messg = '';
        if (message) {
            messg = (<div style={{textAlign: 'center'}} className="alert alert-danger" role="alert">
            {message}
          </div>)
        }
        const styles = {
            marginTop : 20
        }
        const imageStyle = {
            width: '50%',
            borderRadius: '50%',
            cursor: 'pointer'
          }

        return (
            <div className="container">
               <div className='row' style={{margin: 15}}>
                    <button onClick={() => this.props.history.goBack()} className='btn btn-sm btn-primary'>Back</button>                     
               </div>
                <div className="row">
                    <div style={styles} className="well col-md-6 col-md-offset-3">
                        <div className="col-md-offset-4" style={{textAlign: 'center'}}>
                        <img onClick={() => this.fileInput.click()} src={`/UploadedImages/${pictureName}`} alt="pic" className="img-responsive" style={imageStyle} />
                        </div>
                        <div style={{textAlign: 'center'}}>
                            <label>Click to change picture</label>
                        </div>
                        <div style={styles} className="col-md-12">
                           <input style={{display: 'none'}} type="file" onChange={onFileChange} ref={fileInput => this.fileInput = fileInput} />
                        </div>
                        <div style={styles} className="col-md-12">
                           <input type="text" name="name" value={name} onChange={onInputChange} placeholder="Name" className="form-control" />
                        </div>
                        <div style={styles} className="col-md-6">
                           <input type="text" name="price" value={price} onChange={onInputChange} placeholder="Price" className="form-control" />
                        </div>
                        <div style={styles} className="col-md-12">
                           <textarea rows="4" cols="100" name="notes" value={notes} onChange={onInputChange} placeholder="Notes..." className="form-control" />
                        </div>
                        <div style={styles} className="col-md-12">
                        <ReactTags
                                tags={tags}
                                suggestions={suggestions}
                                handleDelete={handleDelete}
                                handleAddition={handleAddition}
                                allowNew={true} 
                        />
                        </div>
                        <div style={styles} className="col-md-12">
                            <table className="table">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th style={{textAlign: 'center'}} >Size</th>
                                        <th style={{textAlign: 'center'}} >Quantity</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {sizes.map((s, idx) => <tr id={idx} key={idx}>
                                        <td>{idx === 0 ? '' : <h5 onClick={() => removeSize(idx)} style={{cursor: 'pointer'}} className="glyphicon glyphicon-remove-sign"></h5>}</td>
                                        <td><input name='size' className='form-control' type='text' value={s.size} placeholder='Size' onChange={(e) => changeSize(e, idx)} /></td>
                                        <td><input name='quantity' className='form-control' type='text' value={s.quantity} placeholder='Quantity' onChange={(e) => changeSize(e, idx)} /></td>
                                    </tr>)}
                                    <tr>
                                        <td style={{textAlign: 'center'}} colSpan={3}><button onClick={addSize} className="btn"><h3 className="glyphicon glyphicon-plus"></h3></button></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div className="col-md-12">
                            {messg}
                        </div>
                        <div style={{marginTop : 30}} className="col-md-12">
                            <button onClick={onAddClick} className="btn btn-info btn-block">Add Product</button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}
