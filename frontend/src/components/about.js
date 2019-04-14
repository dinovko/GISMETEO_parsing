import React from 'react';
import PropTypes from 'prop-types';
import { withStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';

const styles = theme => ({
  root: {
    ...theme.mixins.gutters(),
    paddingTop: theme.spacing.unit * 2,
    paddingBottom: theme.spacing.unit * 2,
  },
});

function PaperSheet(props) {
  const { classes } = props;

  return (
    <div>
      <Paper className={classes.root} elevation={1}>
        <Typography variant="h5" component="h3">
          ЗАДАНИЕ:
        </Typography>
        <a href="https://www.gismeteo.kz/" target="_blank" rel="noopener noreferrer">GISMETEO.KZ</a>
        <Typography component="p">
            Необходимо распарсить данные(влажность температура и тп) за три дня с сайта gismeteo.kz
            И разложить в базу данных.
            В приложении должна быть возможность выбора города.
        </Typography>
        <Typography component="p">
            для подключения БД, необходимо запустить back-end => миграция (Add-migrations "initDB") => update-database
        </Typography>
      </Paper>
    </div>
  );
}

PaperSheet.propTypes = {
  classes: PropTypes.object.isRequired,
};

export default withStyles(styles)(PaperSheet);