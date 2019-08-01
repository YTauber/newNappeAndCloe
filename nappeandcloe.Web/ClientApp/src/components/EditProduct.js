import React, { Component } from 'react';
import { format } from 'money-formatter';
import Modal from 'react-modal';
import ReactTags from 'react-tag-autocomplete';
import {produce} from 'immer';
import axios from 'axios';


Modal.setAppElement(document.getElementById('root'));


export default class EditProduct extends Component {
    state = {

        modalIsOpen: false,


        editProduct: {
            id : '',
            name : '',
            price : '',
            quantity :'',
            notes : '',
            pictureName: '',
            productLabels: [],
            
        },
        tags: [],
        labels : [],
        message: '',
        loading : true,
        amount : ''
    }

    componentDidMount = () => {

        axios.get(`/api/product/getproductbyid/${this.props.match.params.id}`).then(({ data }) => {
            
            this.setState({ editProduct: data, loading: false });
            console.log(this.state.product)
        });

        this.setlabels();
    }

    setlabels = () => {
        axios.get('/api/product/getalllabels').then(({ data }) => {
            
            this.setState({ labels: data });
        });

    }


    edit = () => {
        const {labels, product} = this.state;
        let tags = labels.filter(l => product.productLabels.some(p => p.labelId === l.id))
        this.setState({modalIsOpen: true, tags, editProduct: this.state.product});
    }

    openModal = () => {
        this.setState({modalIsOpen: true});
      }
     
      closeModal = () => {
        this.setState({modalIsOpen: false, message : ''});
      }

      onFileChange = (e) => {

        const formData = new FormData();
        formData.append('file', e.target.files[0])

        axios.post('api/product/addpicture' , formData).then(({data}) => {
            
            const nextState = produce(this.state, draft => {
                draft.editProduct.pictureName = data
            });
            this.setState(nextState);

        })
        
        
    }

    onInputChange = e => {
        const nextState = produce(this.state, draft => {
            draft.editProduct[e.target.name] = e.target.value;
        });
        this.setState(nextState);
    }

    update = () => {

        let {tags, editProduct} = this.state;

        let {id, name, price ,quantity, notes, pictureName, productLabels} = editProduct;

        if (!name){
            this.setState({message : 'You didn\'t enter a valid name... :('})
        }
        
        else if (!quantity || isNaN(quantity)){
            this.setState({message : 'You didn\'t enter a valid quantity... :('})
        }

        else if (!price || isNaN(price)){
            this.setState({message : 'You didn\'t enter a valid price... :('})
        }

        else {
            
        tags = tags.map(t => t.name)
        
        axios.post('api/product/addtags', {tags}).then(({data}) => {
            editProduct.productLabels.length = 0;
            data.forEach(d => editProduct.productLabels.push({productId: editProduct.id, labelId: d.id}));
            
            this.setlabels();
            axios.post('api/product/updateproduct', {id, name, price ,quantity, notes, pictureName, productLabels}).then(() => {

                this.setState({message :'', product: editProduct});
            }
        )})
        
        this.closeModal();
        this.forceUpdate();
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



    render() {

        let messg = '';
        if (message) {
            messg = (<div style={{textAlign: 'center'}} className="alert alert-danger" role="alert">
            {message}
          </div>)
        }


        return (
            <div>
                       <Modal
            className="col-md-3 well"
              isOpen={modalIsOpen}
              onRequestClose={closeModal}
              style={customStyles}
              contentLabel="Example Modal"
              center={true}
              blockScroll={false}
            >
                
                <div className="col-md-12">
                            {messg}
                        </div>
                        <div className="col-md-4 col-md-offset-4">
                        <img onClick={() => this.fileInput.click()} src={`/UploadedImages/${editProduct.pictureName}`} alt="pic" className="img-responsive"
                                        style={{borderRadius: '50%', maxHeight: '100px', cursor : 'pointer'}} />
                        </div>
                        <div style={{textAlign: 'center'}}>
                            <label>Click to change picture</label>
                        </div>
                        <div style={{marginTop: '15px'}} className="col-md-12">
                           <input style={{display: 'none'}} type="file" onChange={onFileChange} ref={fileInput => this.fileInput = fileInput} />
                        </div>
                        <div style={{marginTop: '15px'}} className="col-md-12">
                           <input type="text" name="name" value={editProduct.name} onChange={onInputChange} placeholder="Name" className="form-control" />
                        </div>
                        <div style={{marginTop: '15px'}} className="col-md-6">
                           <input type="text" name="quantity" value={editProduct.quantity} onChange={onInputChange} placeholder="Quantity" className="form-control" />
                        </div>
                        <div style={{marginTop: '15px'}} className="col-md-6">
                           <input type="text" name="price" value={editProduct.price} onChange={onInputChange} placeholder="Price" className="form-control" />
                        </div>
                        <div style={{marginTop: '15px'}} className="col-md-12">
                           <textarea rows="4" cols="100" name="notes" value={editProduct.notes} onChange={onInputChange} placeholder="Notes..." className="form-control" />
                        </div>
                        <div style={{marginTop: '15px'}} className="col-md-12">
                        <ReactTags
                                tags={tags}
                                suggestions={labels}
                                handleDelete={handleDelete}
                                handleAddition={handleAddition}
                                allowNew={true} 
                        />
                        </div>
                        <div style={{marginTop : 30}} className="col-md-12">
                            <div className="col-md-6">
                                <button onClick={update} className="btn btn-info btn-block">Update</button>
                            </div>
                            <div className="col-md-6">
                                <button onClick={closeModal} className="btn btn-info btn-block">Cancel</button>
                            </div>
                        </div>
               
            </Modal>
            </div>
        )
    }
}
