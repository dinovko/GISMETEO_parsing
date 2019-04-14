import React from 'react'
import ReactDOM from 'react-dom'
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import Button from '@material-ui/core/Button';
import Input from '@material-ui/core/Input';
import OutlinedInput from '@material-ui/core/OutlinedInput';
import FilledInput from '@material-ui/core/FilledInput';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormHelperText from '@material-ui/core/FormHelperText';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import CircularProgress from '@material-ui/core/CircularProgress';
import Snackbar from '@material-ui/core/Snackbar';
import RService from '../services/service'
import Weather from './weather'


const styles = theme => ({
    button: {
      margin: theme.spacing.unit,
    },
    input: {
      display: 'none',
    },
    root: {
      display: 'flex',
      flexWrap: 'wrap',
    },
    formControl: {
      margin: theme.spacing.unit,
      minWidth: 120,
    },
    selectEmpty: {
      marginTop: theme.spacing.unit * 2,
    },
  });

class GetCity extends React.Component {
state = {
    data:null,
    age: '',
    name: 'hai',
    labelWidth: 0,
    cityList:"",
    cityId:0,
    selectedCity:"",
    isLoading: false,
    open: false,
    vertical: 'bottom',
    horizontal: 'right',
    smsg:'данные загружены',
}

    handleClick = state => () => {
        this.setState({ open: true, ...state });
      };
    
      handleClose = () => {
        this.setState({ open: false });
      };

    handleLoadCities = () => {
      this.setState({isLoading:true});
        const url = 'cities/parse/list';
    RService.get(`${url}`)
      .then(response => {
        if(!response.isError) {
          this.setState({cityList: response.content, isLoading:false, smsg:'города загружены', open:true });
        }
      });
    }

    handleGetWeather = (refer,id) => {
        this.setState({isLoading:true});
        const url = 'weathers/parse';
         RService.get(`${url}${refer}${id}`)
          .then(response => {
            if(!response.isError) {
            this.setState({data: response.content, isLoading:false, smsg:'погода загружена', open:true  });
          }
        });
      }

    handleChange = event => {
        this.setState({ [event.target.name]: event.target.value });
    };

render(){
    const { classes } = this.props;
    const {cityList, isLoading, selectedCity, data, vertical, horizontal, open, smsg} = this.state;
    return(
        <div>
            <Button 
                variant="contained" 
                color="primary"
                onClick={this.handleLoadCities} 
                className={classes.button}>
                загрузить города
            </Button>
            <Button 
                variant="contained" 
                color="primary"
                onClick={() => this.handleGetWeather(selectedCity, 0)} 
                className={classes.button}>
                получить погоду
            </Button><br/>
                {
                    isLoading &&
                    <CircularProgress disableShrink />
                }
            <br/><FormControl className={classes.formControl}>
          <InputLabel shrink htmlFor="city-label-placeholder">
            город
          </InputLabel>
          <Select
            value={selectedCity}
            onChange={this.handleChange}
            input={<Input name="city" id="city-label-placeholder" />}
            displayEmpty
            name="selectedCity"
            className={classes.selectEmpty}
          >
            <MenuItem value="">
              <em>город</em>
            </MenuItem>
            {
                cityList && cityList.map((data) => (
                    <MenuItem key={data.id} value={data.reference}>{data.cityName}</MenuItem>
                ))
            }
            
          </Select>
          <FormHelperText>выберите город</FormHelperText>
        </FormControl>
            <div>
                { data &&
                    <Weather refer={data}/>
                }
            </div>
            <Snackbar
                anchorOrigin={{ vertical, horizontal }}
                open={open}
                onClose={this.handleClose}
                ContentProps={{
                    'aria-describedby': 'message-id',
                }}
                message={<span id="message-id">{smsg}</span>}
            />
        </div>
    )
}

}

GetCity.propTypes = {
    classes: PropTypes.object.isRequired,
  };

export default withStyles(styles)(GetCity);